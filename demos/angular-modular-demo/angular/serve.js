const { execSync } = require('child_process');
const port = process.env.PORT || 4200;
execSync(`npx ng serve --port=${port}`, { stdio: 'inherit' });
