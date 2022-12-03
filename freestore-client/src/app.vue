<template>
  <modal-screen v-if="modalData" :message="modalData.message" :onConfirm="modalData.onConfirm" @closeModal="modalData = null"/>
  <mainHeader :authenticated="userStatus.authenticated" :userName="(userStatus.userData ? (userStatus.userData.name + ' ' + userStatus.userData.last_name) : null)" :level="userStatus.userData?.level" />
  <component :is="currentView" @triggerModal="(message, onConfirm) => modalData = { message: message, onConfirm: onConfirm }"/>
</template>

<script>
import loginScreen from './screens/loginScreen.vue'
import homeScreen from './screens/homeScreen.vue'
import notFoundScreen from './screens/notFoundScreen.vue'
import mainHeader from './misc/mainHeader.vue'
import profileScreen from './screens/profileScreen.vue'
import registerScreen from './screens/registerScreen.vue'
import profileEditScreen from './screens/profileEditScreen.vue'
import postFeedScreen from './screens/postFeedScreen.vue'
import postDetailsScreen from './screens/postDetailsScreen.vue'
import postCreateScreen from './screens/postCreateScreen.vue'
import usersScreen from './screens/usersScreen.vue'
import modalScreen from './components/informational/modalScreen.vue'

const routes = {
  '/': homeScreen,
  'accounts': usersScreen,
  'login': loginScreen,
  'profile': {
    '/': profileScreen,
    'edit': profileEditScreen
  },
  'register': registerScreen,
  'newItem': postCreateScreen,
  'categories': {
    '/': notFoundScreen,
    'new': notFoundScreen,
    '*': {
      '/': notFoundScreen,
      'items': {
        '/': postFeedScreen,
        '*': postDetailsScreen,
      },
      'edit': notFoundScreen
    }
  }
}

export default {
  name: 'app',
  components: {
    mainHeader,
    modalScreen
  },
  data() {
    return {
      modalData: null,
      currentPath: window.location.pathname,
      userStatus: {
        authenticated: false
      }
    }
  },
  emits: ["triggerModal", "closeModal"],
  computed: {
    currentView() {
      var endpoints = this.currentPath.split("/")
      endpoints.shift()
      if (endpoints[endpoints.length - 1] === "") {
        endpoints.pop()
      }
      var currEntry = endpoints.shift() || undefined
      var currRouteElem = routes
      while (currEntry != undefined && currRouteElem != undefined) {
        currRouteElem = currRouteElem[currEntry] || currRouteElem['*']
        currEntry = endpoints.shift()
      }

      return currEntry === undefined && (currRouteElem?.__file ?? false) ?
        currRouteElem : (currRouteElem?.['/'] || notFoundScreen)
    },

    header() {
      return mainHeader
    }
  },
  async beforeMount() {
    if (!this.userStatus.authenticated) {
      var token = localStorage.getItem("token")
      if (!token) {
        return
      }

      var response = await this.performRequest("/accounts/own", "GET")
      if (response.success) {
        this.updateHeaderInfo(response.body)
        return
      }

      localStorage.clear()
    }

    this.updateHeaderInfo()
  },
  mounted() {
    window.addEventListener('hashchange', () => {
      this.currentPath = window.location.hash
    })
  },
  methods: {
    update() {
      console.log("app updated")
      this.$forceUpdate()
    },

    updateHeaderInfo(data) {
      this.userStatus.authenticated = data != null
      this.userStatus.userData = data
    }
  }
}
</script>

<style>
body {
  background-color: #373b3b;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #a1b6ca;
}

.refButton {
  margin: 20px;
  border-radius: 10px;
  padding: 10px;
  background-color: #91a1a1;
  max-width: 250px;
  display: inline-block;
  border: none;
  color: rgb(42, 70, 68)
}

.refButton:hover {
  background-color: #57a1a1;
  color: white;
}

h2 {
  color: rgb(42, 70, 68)
}
</style>
