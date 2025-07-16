# Cronjob Configuration and Documentation

This repository contains a set of cronjobs designed to handle specific tasks related to business logic. Each cronjob is associated with its own configuration file located in the `business_logic` directory.

## Table of Contents
1. [Cronjob Descriptions](#cronjob-descriptions)
2. [Configuration Files](#configuration-files)
3. [How to Use](#how-to-use)

---

## Cronjob Descriptions

### 1. **Comparison Cronjob**
- **Script Name:** `cronjob1`
- **Purpose:** Handles comparison-related operations across datasets or components.
- **Configuration File:** `config1.yaml`

### 2. **General Context Cronjob**
- **Script Name:** `cronjob2`
- **Purpose:** Manages general context updates and ensures synchronization of shared data.
- **Configuration File:** `config2.yaml`

### 3. **Project Specific General Index Cronjob**
- **Script Name:** `cronjob3`
- **Purpose:** Builds and maintains a general index for project-specific data.
- **Configuration File:** `config3.yaml`

### 4. **Project Specific Cronjobs**
- **Script Name:** `cronjob4`
- **Purpose:** Builds and maintains project-specific data with multi index approach.
- **Configuration File:** `config4.yaml`

---

## Configuration Files

All configuration files are located in the following directory:

business_logic/
| Cronjob                    | Configuration File |
|----------------------------|--------------------|
| Comparison Cronjob         | `config1.yaml`    |
| General Context Cronjob    | `config2.yaml`    |
| General Index Cronjob      | `config3.yaml`    |
| Project Specific Cronjobs  | `config4.yaml`    |

Each configuration file defines the parameters and settings necessary for its respective cronjob to function effectively. 

---

## How to Use

1. **Locate Configuration Files:**
   - Ensure all required `config*.yaml` files are present in the `business_logic/` directory.
   
2. **Run the Cronjobs:**
   - Execute each cronjob according to its schedule or requirements.

   Example (for `cronjob1`):
   poner ejemplo