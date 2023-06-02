const {env} = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:22114';

const PROXY_CONFIG = [
  {
    context: [
      "/telegramintegration/login",
      "/telegramintegration/reconnect",
      "/telegramintegration/disconnect",
      "/telegramintegration/message",
      "/telegramintegration/get-chat",
      "/telegramintegration/send-media",
      "/telegramintegration/schedule-message",
      "/telegramintegration/multiple-at-once"
    ],
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  }
]

module.exports = PROXY_CONFIG;
