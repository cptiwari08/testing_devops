FROM python:3.11.7

WORKDIR /code

# Python requirements
RUN pip install --upgrade pip
COPY ./requirements.txt /code/requirements.txt
RUN pip install --no-cache-dir --upgrade -r /code/requirements.txt
RUN python -m nltk.downloader popular

# OS requirements

# Adding Microsoft repository for the ODBC Driver
RUN curl https://packages.microsoft.com/keys/microsoft.asc |  tee /etc/apt/trusted.gpg.d/microsoft.asc
RUN curl https://packages.microsoft.com/config/debian/11/prod.list |  tee /etc/apt/sources.list.d/mssql-release.list


# Install the Microsoft ODBC Driver for SQL Server
RUN DEBIAN_FRONTEND=noninteractive && apt-get update && \
    ACCEPT_EULA=Y apt-get install -y --no-install-recommends msodbcsql17 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* && \
    unset DEBIAN_FRONTEND


# Uninstall unused vulnerable packages
RUN apt remove --purge imagemagick git -y
RUN apt autoremove -y

# copy the app
COPY ./app /code/app
COPY ./tenacity.yaml /code/tenacity.yaml

EXPOSE 8000

RUN useradd -m nonroot
USER nonroot

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000", "--loop", "asyncio" ]
