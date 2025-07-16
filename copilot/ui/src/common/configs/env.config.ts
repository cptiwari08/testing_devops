import prodConfig from './env.config.prod';
import devConfig from './env.config.dev';

const env = process.env.NODE_ENV || 'development';
const config = env === 'production' ? prodConfig : devConfig;

export default config;