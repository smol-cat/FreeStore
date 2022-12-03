<template>
    <div class="header">
        <div class="side">
            <loginHRef :userName="this.userName" />
        </div>
        <div class="mainTitle">
            <h1>FreeStore</h1>
        </div>
        <div class="navBarArea">
            <mainNavBar :authenticated="this.authenticated" :level="this.level" />
        </div>
    </div>
</template>

<script>
import mainNavBar from '@/components/navigation/mainNavBar.vue';
import loginHRef from '@/components/navigation/loginHRef.vue';

export default {
    name: "mainHeader",
    props: {
        authenticated: Boolean,
        userName: String,
        level: Number,
    },
    components: {
        mainNavBar,
        loginHRef
    },
}
</script>

<style>
div .side {
    margin: 20px;
    float: right;
    text-align: right;
}

.mainTitle {
    color: rgb(200, 255, 255);
    text-align: left;
    margin: 10px;
    /* padding: 10px; */
    font-size: 25px;
    font-family: Haettenschweiler, 'Arial Narrow Bold', sans-serif;
}

div .header {
    margin: auto;
    display: block;
    background-color: #91a1a1;
    border-radius: 10px;
    max-width: 1200px;
}

.navBarArea{
    width: inherit;
}
</style>