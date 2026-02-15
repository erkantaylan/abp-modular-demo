const { execSync } = require('child_process');
const fs = require('fs');
const path = require('path');

const port = process.env.PORT || 4200;

// Aspire injects service URLs as env vars via WithReference
// Format: services__<name>__<scheme>__0 = http://host:port
const apiUrl = Object.entries(process.env)
  .filter(([k]) => k.startsWith('services__') && k.includes('httpapi'))
  .map(([, v]) => v)
  .find(v => v?.startsWith('https://')) || 'https://localhost:44340';

const angularUrl = `http://localhost:${port}`;

// Generate environment.ts with Aspire-provided URLs
const envContent = `import { Environment } from '@abp/ng.core';

const baseUrl = '${angularUrl}';

const oAuthConfig = {
  issuer: '${apiUrl}/',
  redirectUri: baseUrl,
  clientId: 'AngularDemo_App',
  responseType: 'code',
  scope: 'offline_access AngularDemo',
  requireHttps: ${apiUrl.startsWith('https')},
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
      url: '${apiUrl}',
      rootNamespace: 'AngularDemo',
    },
    AbpAccountPublic: {
      url: oAuthConfig.issuer,
      rootNamespace: 'AbpAccountPublic',
    },
  },
} as Environment;
`;

const envPath = path.join(__dirname, 'src', 'environments', 'environment.ts');
fs.writeFileSync(envPath, envContent);
console.log(`Angular environment: API=${apiUrl}, SPA=${angularUrl}`);

execSync(`npx ng serve --port=${port}`, { stdio: 'inherit' });
