import json
import traceback
from typing import Type, TypeVar

import redis
from app.core.config import RedisConfig
from app.core.logging_config import LoggerConfig
from app.core.schemas import RuntimeStatus
from fastapi import HTTPException, status
from pydantic import BaseModel
from redis.sentinel import Sentinel
from redis.typing import ResponseT

T = TypeVar("T", bound=BaseModel)


class RedisService:
    def __init__(self, redis_config: RedisConfig, logger: LoggerConfig) -> None:
        self.logger: LoggerConfig = logger
        if redis_config.localhost:
            self.client = redis.Redis(
                host="localhost",
                port=6379,
                decode_responses=True,
                charset="utf-8",
                socket_timeout=20,  # Timeout after 20 seconds
            )
            self.logger.info("Connected to Redis. Host: localhost")
        else:
            sentinel = Sentinel(
                [(redis_config.url, redis_config.sentinel_port)],
                sentinel_kwargs={"password": redis_config.password},
            )

            host, port = sentinel.discover_master(redis_config.sentinel_service_name)
            self.client = redis.Redis(
                host=host,
                port=port,
                password=redis_config.password,
                decode_responses=True,
                charset="utf-8",
                socket_timeout=20,  # Timeout after 20 seconds
            )
            self.logger.info(f"Connected to Redis Sentinel. Host: {host}, Port: {port}")

    def exists(self, key) -> ResponseT:
        return self.client.exists(key)

    def get(self, key, m: Type[T]) -> T | None:
        try:
            model_str = str(self.client.get(key))
            if not model_str:
                return None
            model_data = json.loads(model_str)
            model: T = m(**model_data)
        except Exception as e:
            self.logger.error(f"An error occurred while getting key: {key}")
            self.logger.error(f"Unhandled exception: {traceback.format_exc()}")
            raise HTTPException(
                detail=f"An unexpected error occurred: {e}",
                status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            )

        return model

    def update(self, key, value: BaseModel) -> ResponseT:
        return self.client.set(key, value.model_dump_json())

    def set(self, key: str, value: str, ex: int = 1800 ) -> ResponseT:
        return self.client.set(key, value, ex=ex)

    def is_workflow_aborted(self, key: str, m: Type[T]) -> bool:
        workflow = self.get(key, m)
        return workflow.runtimeStatus == RuntimeStatus.aborted  # type: ignore
