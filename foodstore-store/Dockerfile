FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM node:22-alpine
WORKDIR /app
ENV NODE_ENV=production
ENV PUBLIC_API_URL=http://api:8080/v2
ENV PUBLIC_AUTH_STORAGE_PREFIX=foodstore_
ENV PUBLIC_SIGNALR_HUB=/hubs/app
ENV PUBLIC_SIGNALR_WS_ONLY=true
ENV PUBLIC_VIETQR_API_URL=https://api.vietqr.io/v2/banks
ENV PUBLIC_DEFAULT_QR_TEMPLATE=compact2
ENV PUBLIC_SITE_DOMAIN=foodstore
ENV PUBLIC_DEFAULT_CUSTOMER_PASSWORD=Customer@123

COPY package*.json ./
RUN npm ci --only=production
COPY --from=build /app/build ./build
EXPOSE 3000
CMD ["node", "build"]
