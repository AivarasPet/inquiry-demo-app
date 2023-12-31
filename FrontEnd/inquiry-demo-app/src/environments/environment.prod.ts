export const environment = {
  production: false,
  get AUTH_URL() { return '/auth'; },
  get SIGNALR_URL() { return '/notify'; },
  get INQUIRIES_URL() { return '/inquiries'; },
  INQUIRIES_REFRESH_TOPIC: 'refresh-inquiries'
};