<template>
    <a v-if="!headerInfo.authenticated" href="/login">Prisijungti</a>
    <p v-if="headerInfo.authenticated">Prisijungėte kaip: <b>{{ this.headerInfo.userData.name + " " +
            this.headerInfo.userData.last_name
    }}</b></p>
    <a v-if="headerInfo.authenticated" v-on:click="logout()" href="#">Atsijungti</a>
</template>

<script>

export default {
    props: {
        headerInfo: Object
    },
    name: "loginHRef",
    methods: {
        async logout() {
            var response = await this.performRequest("/tokens", "DELETE")
            if (response.success) {
                sessionStorage.clear()
                localStorage.clear()
                location.pathname = ""
            }
        }
    }
}
</script>

<style>
p {
    color: rgb(42, 70, 68);
    margin: 0px;
}

a {
    color: rgb(44, 42, 70);
    text-decoration: none;
}

a:hover {
    color: rgb(110, 108, 136),

}
</style>