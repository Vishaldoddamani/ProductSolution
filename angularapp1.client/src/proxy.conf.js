const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'https://localhost:7157/api/product';

const PROXY_CONFIG = [
  {
    context: [
      "/api/product",
    ],
    target,
    secure: true
  }
]

module.exports = PROXY_CONFIG; 
