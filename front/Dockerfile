FROM node:17.6-alpine AS build
WORKDIR /source
EXPOSE 8080

COPY . /source

RUN yarn install
RUN yarn build

FROM node:17.6-alpine
WORKDIR /app
RUN yarn global add http-server
COPY --from=build /source/public /app
CMD ["http-server", "/app"]
