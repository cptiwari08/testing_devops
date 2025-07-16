import yaml
import os
import glob

class DynamicSettings:
    def __init__(self, directory='business_logic'):
        self.configs = {}
        self._load_all_settings(directory)

    def _load_all_settings(self, directory):
        yaml_files = glob.glob(os.path.join(directory, 'config*.yaml'))
        for path in yaml_files:
            filename = os.path.basename(path)
            identifier = int(''.join(filter(str.isdigit, filename)))
            with open(path, 'r') as file:
                config = yaml.safe_load(file)
                self.configs[identifier] = {}
                self._load_variables(config, self.configs[identifier])

    def _load_variables(self, config, container, prefix=''):
        for key, value in config.items():
            if isinstance(value, dict):
                self._load_variables(value, container, f"{prefix}{key}.")
            else:
                container[f"{prefix}{key}"] = value

    def get_settings(self, identifier):
        return self.configs.get(identifier, None)

    def get_variable(self, identifier, variable_name):
        settings = self.get_settings(identifier)
        if settings:
            return settings.get(variable_name, None)
        return None