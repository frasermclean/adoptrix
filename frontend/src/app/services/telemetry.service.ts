import { Injectable } from '@angular/core';
import { ApplicationInsights } from '@microsoft/applicationinsights-web';

import { AngularPlugin } from '@microsoft/applicationinsights-angularplugin-js';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class TelemetryService {
  private appInsights: ApplicationInsights;

  constructor(router: Router) {
    const angularPlugin = new AngularPlugin();
    this.appInsights = new ApplicationInsights({
      config: {
        connectionString: environment.appInsights.connectionString,
        enableCorsCorrelation: true,
        extensions: [angularPlugin],
        extensionConfig: {
          [angularPlugin.identifier]: { router: router },
        },
      },
    });
  }

  public initialize(): void {
    this.appInsights.loadAppInsights();

    this.appInsights.addTelemetryInitializer((item) => {
      item.tags = item.tags || [];
      item.tags['ai.cloud.role'] = 'adoptrix-frontend';
    });
  }
}
