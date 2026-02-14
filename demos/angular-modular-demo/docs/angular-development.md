# Angular Development Guide

This guide covers Angular-specific development patterns and workflows for the AngularDemo project.

## Project Structure

```
angular/
├── src/
│   ├── app/
│   │   ├── home/                    # Home page component
│   │   ├── proxy/                   # Auto-generated API proxy services
│   │   ├── route.provider.ts        # Application route configuration
│   │   ├── app.component.ts         # Root component
│   │   ├── app.module.ts            # Root module
│   │   └── app-routing.module.ts    # Root routing
│   ├── environments/
│   │   ├── environment.ts           # Development config (API URL, OAuth)
│   │   └── environment.prod.ts      # Production config
│   └── assets/                      # Static assets
├── e2e/                             # End-to-end tests
├── angular.json                     # Angular CLI configuration
├── package.json                     # npm dependencies
├── tsconfig.json                    # TypeScript configuration
└── proxy.conf.json                  # Dev server proxy (if configured)
```

## Development Workflow

### Starting the Dev Server

```bash
cd angular
npm start
```

This starts the Angular CLI dev server on `http://localhost:4200` with live reload enabled. The API Host must be running separately on `https://localhost:44340`.

### Building for Production

```bash
cd angular
npm run build -- --configuration production
```

Output goes to `angular/dist/`. This can be served by any static file server or CDN.

## API Communication

### How Angular Talks to the Backend

The Angular app communicates with the .NET API via REST/JSON. ABP provides two mechanisms:

**1. Auto-generated API Proxies**

ABP can generate TypeScript proxy services from your backend API. Run:

```bash
abp generate-proxy -t ng
```

This creates TypeScript services in `src/app/proxy/` that mirror your backend application services. You get strongly-typed methods like:

```typescript
// Auto-generated — don't edit manually
@Injectable({ providedIn: 'root' })
export class BookService {
  getList(input: PagedAndSortedResultRequestDto): Observable<PagedResultDto<BookDto>> {
    return this.restService.request({ ... });
  }
}
```

**2. Direct REST Calls**

You can also use ABP's `RestService` directly:

```typescript
import { RestService } from '@abp/ng.core';

@Injectable({ providedIn: 'root' })
export class MyService {
  constructor(private rest: RestService) {}

  getBooks() {
    return this.rest.request<void, BookDto[]>({
      method: 'GET',
      url: '/api/app/books',
    });
  }
}
```

### Environment Configuration

API URLs are configured in `src/environments/environment.ts`:

```typescript
export const environment = {
  production: false,
  application: {
    baseUrl: 'http://localhost:4200',
    name: 'AngularDemo',
  },
  oAuthConfig: {
    issuer: 'https://localhost:44340/',
    redirectUri: 'http://localhost:4200',
    clientId: 'AngularDemo_App',
    responseType: 'code',
    scope: 'offline_access AngularDemo',
  },
  apis: {
    default: {
      url: 'https://localhost:44340',
      rootNamespace: 'AngularDemo',
    },
  },
};
```

## Authentication (OAuth 2.0)

The Angular app uses **Authorization Code Flow with PKCE** to authenticate against the API's OpenIddict server.

Flow:
1. User clicks "Login" in Angular
2. Angular redirects to `https://localhost:44340/connect/authorize`
3. User logs in on the server-rendered login page
4. Server redirects back to `http://localhost:4200` with an authorization code
5. Angular exchanges the code for an access token
6. All subsequent API requests include the token in the `Authorization` header

The OAuth configuration is in `environment.ts` under `oAuthConfig`.

## ABP Angular Modules

ABP provides pre-built Angular modules for common functionality:

| Module | Package | Purpose |
|--------|---------|---------|
| Core | `@abp/ng.core` | Base services, REST client, auth |
| Theme LeptonX Lite | `@abp/ng.theme.lepton-x` | UI theme and layout |
| Account | `@abp/ng.account` | Login, register, profile |
| Identity | `@abp/ng.identity` | User/role management UI |
| Tenant Management | `@abp/ng.tenant-management` | Multi-tenant management UI |
| Setting Management | `@abp/ng.setting-management` | Application settings UI |

These are configured in `app.module.ts` and provide ready-to-use pages for administration.

## Creating a New Feature Module

### 1. Generate the Module

```bash
cd angular
npx ng generate module books --route books --module app.module
```

This creates:
- `src/app/books/books.module.ts` — Feature module
- `src/app/books/books-routing.module.ts` — Route configuration
- `src/app/books/books.component.ts` — Default component

### 2. Generate API Proxies

After creating backend services, generate TypeScript proxies:

```bash
# From the solution root
abp generate-proxy -t ng
```

### 3. Add Navigation

Register the route in `src/app/route.provider.ts`:

```typescript
import { RoutesService, eLayoutType } from '@abp/ng.core';

export class AppRoutingModule {
  constructor(private routes: RoutesService) {
    routes.add([
      {
        path: '/books',
        name: 'Books',
        iconClass: 'fas fa-book',
        order: 2,
        layout: eLayoutType.application,
      },
    ]);
  }
}
```

### 4. Use ABP UI Components

ABP provides Angular UI components for common patterns:

```typescript
import { ListService, PagedResultDto } from '@abp/ng.core';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';

@Component({
  selector: 'app-books',
  template: `
    <abp-page [title]="'Books' | abpLocalization">
      <abp-page-toolbar>
        <button class="btn btn-primary" (click)="createBook()">
          {{ 'NewBook' | abpLocalization }}
        </button>
      </abp-page-toolbar>
      <!-- Table, forms, etc. -->
    </abp-page>
  `,
  providers: [ListService],
})
export class BooksComponent {
  constructor(
    public list: ListService,
    private bookService: BookService,
    private confirmation: ConfirmationService
  ) {}
}
```

## Localization

ABP's localization system works across both frontend and backend. Localization resources defined in `Domain.Shared` are available in Angular via the `abpLocalization` pipe:

```html
<h1>{{ '::Welcome' | abpLocalization }}</h1>
<p>{{ '::LongText' | abpLocalization : 'param1' : 'param2' }}</p>
```

To add new localization strings, edit the JSON files in:
`src/AngularDemo.Domain.Shared/Localization/AngularDemo/`

## Testing

### Unit Tests

```bash
cd angular
npm test
```

Uses Karma + Jasmine by default (configured in `karma.conf.js`).

### End-to-End Tests

```bash
cd angular
npx ng e2e
```

E2E test files are in the `e2e/` directory.

## Common Tasks

### Update ABP Packages

```bash
# Update .NET packages
abp update

# Update Angular packages
cd angular
npm update @abp/ng.core @abp/ng.theme.lepton-x @abp/ng.account @abp/ng.identity @abp/ng.tenant-management @abp/ng.setting-management
```

### Regenerate API Proxies

After changing backend API signatures:

```bash
abp generate-proxy -t ng
```

### Add a New ABP Module

```bash
# Install the .NET module
abp add-module Volo.Blogging

# Then regenerate Angular proxies
abp generate-proxy -t ng
```
