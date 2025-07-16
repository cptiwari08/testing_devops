# Base image
FROM python:3.12

# Working Directory
WORKDIR /app

COPY ./requirements/requeriments_gi.txt /app/requirements.txt

# Install requirements.txt
RUN pip install --no-cache-dir -r requirements.txt

# Remove git
RUN apt-get remove --purge -y git && \
    apt-get autoremove -y

# Copy ingest and index scripts
COPY ./Indexes/index_core_functions_general.py /app/index_core_functions_general.py
COPY ./Indexes/prj_glossary.json /app/prj_glossary.json
COPY ./Indexes/index_project_specific_cronjob_general.py /app/index_project_specific_cronjob_general.py
COPY logger.py /app/logger.py


RUN useradd -m nonroot
USER nonroot

# Run scripts
CMD python /app/index_project_specific_cronjob_general.py
