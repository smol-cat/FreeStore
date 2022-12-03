import { createApp } from 'vue'
import app from './app.vue'

var myApp = createApp(app)
myApp.config.globalProperties.apiRoot = "http://localhost:5189/api/v1"
myApp.config.globalProperties.performRequest = async (uri, method, body) => {
    var headers = {
        'Content-Type': 'application/json'
    }

    if (localStorage.getItem("token")) {
        headers["Authorization"] = "Bearer " + localStorage.getItem("token")
    }

    try {
        var response = await fetch(myApp.config.globalProperties.apiRoot + uri, {
            method: method,
            mode: 'cors',
            cache: 'no-cache',
            headers: headers,
            redirect: 'follow',
            body: JSON.stringify(body)
        })

        var jsonBody

        try {
            jsonBody = await response.json()
        }
        catch {
            jsonBody = {}
        }
    }
    catch(e){
        return {
            success: false
        }
    }

    console.log(response)

    return {
        success: response.status >= 200 && response.status < 400,
        statusCode: response.status,
        body: jsonBody,
        headers: response.headers
    }
}

myApp.mount('#app')
