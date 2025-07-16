FROM python:3.12

WORKDIR /code

# Python requirements
RUN pip install --upgrade pip
COPY ./requirements.txt /code/requirements.txt
RUN pip install --no-cache-dir --upgrade -r /code/requirements.txt

RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg
RUN curl https://packages.microsoft.com/config/debian/12/prod.list | tee /etc/apt/sources.list.d/mssql-release.list

RUN apt update

RUN ACCEPT_EULA=Y apt install -y redis-tools msodbcsql17 && \
  apt clean && \
  rm -rf /var/lib/apt/lists/*

# Uninstall unused vulnerable packages
RUN apt remove --purge imagemagick git -y
RUN apt autoremove -y

# copy the app
COPY ./app /code/app

EXPOSE 8000

RUN useradd -m nonroot
USER nonroot

CMD ["uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000" ]
