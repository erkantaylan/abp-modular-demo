import { Environment } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

const oAuthConfig = {
  issuer: 'https://localhost:44340/',
  redirectUri: baseUrl,
  clientId: 'AngularDemo_App',
  responseType: 'code',
  scope: 'offline_access AngularDemo',
  requireHttps: true,
};

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: 'AngularDemo',
  },
  oAuthConfig,
  apis: {
    default: {
      url: 'https://localhost:44340',
      rootNamespace: 'AngularDemo',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
