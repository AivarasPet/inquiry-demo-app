// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { InjectionToken } from "@angular/core";
import { IConfig } from "src/app/config.interface";

export const APP_CONFIG = new InjectionToken<IConfig>('environment.configuration');

export const environment = {
  production: false,
  get AUTH_URL() { return '/auth'; },
  get SIGNALR_URL() { return '/notify'; },
  get INQUIRIES_URL() { return '/inquiries'; },
  INQUIRIES_REFRESH_TOPIC: 'refresh-inquiries'
};