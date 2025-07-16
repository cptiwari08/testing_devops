# Use an official Node.js runtime as the base image
FROM node:18-alpine as build

WORKDIR /app

COPY package.json .
COPY .npmrc ./
RUN npm config set registry http://registry.npmjs.org/

RUN sed -i -e  's#\[PUBLIC_URL\]#'"${PUBLIC_URL}"'#g' package.json

RUN npm install --force

COPY . .

RUN npm run build

# STAGE-2
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf
COPY replace_environment_var.sh /usr/share/nginx/html/
RUN chmod +x /usr/share/nginx/html/replace_environment_var.sh

EXPOSE 80

ENTRYPOINT ["sh", "/usr/share/nginx/html/replace_environment_var.sh"]


