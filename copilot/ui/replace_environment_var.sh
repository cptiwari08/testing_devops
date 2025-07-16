echo "Configuring react app to use environmnet specific values."

sed -i -e  's#\[PUBLIC_URL\]#'"${PUBLIC_URL}"'#g' /usr/share/nginx/html/index.html
sed -i -e  's#\[REACT_APP_URL\]#'"${REACT_APP_URL}"'#g' /usr/share/nginx/html/index.html
sed -i -e  's#\[PUBLIC_URL\]#'"${PUBLIC_URL}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[PUBLIC_URL\]#'"${PUBLIC_URL}"'#g' /usr/share/nginx/html/static/css/main*.css
sed -i -e  's#\[REACT_APP_URL\]#'"${REACT_APP_URL}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[REACT_APP_API_URL\]#'"${REACT_APP_API_URL}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[REACT_APP_ENGAGE_MGMT_GUIDANCE_URL\]#'"${REACT_APP_ENGAGE_MGMT_GUIDANCE_URL}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[AZURE_AD_AUTHORITY\]#'"${AZURE_AD_AUTHORITY}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[AZURE_AD_CLIENT_ID\]#'"${AZURE_AD_CLIENT_ID}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[AZURE_AD_APP_SCOPE\]#'"${AZURE_AD_APP_SCOPE}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[AZURE_AD_API_SCOPE\]#'"${AZURE_AD_API_SCOPE}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[TOKEN_RENEWAL_OFFSET_SECONDS\]#'"${TOKEN_RENEWAL_OFFSET_SECONDS}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[PROJECT_ID\]#'"${PROJECT_ID}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[SSP_API_URL\]#'"${SSP_API_URL}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[EY_AI_PRINCIPLES\]#'"${EY_AI_PRINCIPLES}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[MICROSOFT_ACCEPTABLE_USE_POLICY_URL\]#'"${MICROSOFT_ACCEPTABLE_USE_POLICY_URL}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[MICROSOFT_CODE_OF_CONDUCT_OPENAI_SERVICES_URL\]#'"${MICROSOFT_CODE_OF_CONDUCT_OPENAI_SERVICES_URL}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[APPLICATION_INSIGHTS_KEY\]#'"${APPLICATION_INSIGHTS_KEY}"'#g' /usr/share/nginx/html/static/js/main*.js
sed -i -e  's#\[AG_GRID_LICENSE_KEY\]#'"${AG_GRID_LICENSE_KEY}"'#g' /usr/share/nginx/html/static/js/main*.js

nginx -g 'daemon off;'
