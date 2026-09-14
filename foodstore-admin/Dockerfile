FROM node:22-alpine AS build
WORKDIR /app
ARG NEXT_PUBLIC_API_URL=http://api.localhost/v2
ENV NEXT_PUBLIC_API_URL=$NEXT_PUBLIC_API_URL
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM node:22-alpine
WORKDIR /app
ENV NODE_ENV=production
ENV NEXT_PUBLIC_API_URL=http://api.localhost/v2
COPY package*.json ./
RUN npm ci --only=production
COPY --from=build /app/.next ./.next
COPY --from=build /app/public ./public
EXPOSE 3000
CMD ["npm", "start"]
