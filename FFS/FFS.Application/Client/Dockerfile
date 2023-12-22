# Build Stage
FROM node:lts-alpine as builder

WORKDIR /app

COPY ./FFS.Application/Client/package.json .
RUN npm install

COPY ./FFS.Application/Client/ .

ARG VITE_FU_FOODY_PUBLIC_API_BASE_URL
ARG VITE_FU_FOODY_PUBLIC_API_BASE_URL_UPLOAD_IMAGE

# Set environment variables using build-time arguments
ENV VITE_FU_FOODY_PUBLIC_API_BASE_URL='http://103.184.112.242:5000'
ENV VITE_FU_FOODY_PUBLIC_API_BASE_URL_UPLOAD_IMAGE='https://api.imgbb.com/1/upload?key=7eef4b3d48ffb2383bf921eeb1dd5117'


RUN npm run build

FROM nginx:alpine
COPY ./FFS.Application/Client/nginx.conf /etc/nginx/conf.d/default.conf
# COPY ./FFS.Application/Client/nginx.conf /etc/nginx/sites-enabled/ffs

COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 3000
CMD ["nginx", "-g", "daemon off;"]
