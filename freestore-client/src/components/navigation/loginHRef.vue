<template>
    <a v-if="!userName" href="/login">Prisijungti</a>
    <p v-if="userName">Prisijungėte kaip: <b>{{ this.userName }}</b></p>
    <a v-if="userName" v-on:click="logout()" href="#">Atsijungti</a>
</template>

<script>

export default {
    props: {
        userName: String
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