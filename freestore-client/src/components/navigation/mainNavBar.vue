<template>
    <ul class="menuBar">
        <li v-for="(link, name) in getElements()" :key="name" class="menuBar_item" :class="{ activeLink: link === this.getPath() }">
            <a :href="link === this.getPath() ? '#' : link">{{ name }}</a>
        </li>
        <li v-if="headerInfo.authenticated" class="menuBar_item" :class="{ activeLink: '/profile' === this.getPath() }">
            <a :href="'/profile' === this.getPath() ? '#' : '/profile'">Profilis</a>
        </li>
        <li class="menuBar_item">
            <categories-drop-down-menu/>
        </li>
    </ul>
</template>

<script>
import categoriesDropDownMenu from './categoriesDropDownMenu.vue'

export default {
  components: { categoriesDropDownMenu },
    data() {
        return {
            elements: [{
                "Namai": "/"
            }]
        }
    },
    props: {
        headerInfo: Object
    },
    beforeMount() {
        this.elements = {
            "Namai": "/"
        }
    },
    methods: {
        getPath(){
            return window.location.pathname
        },
        getElements() {
            return this.elements
        }
    },
    name: "mainNavBar",
}
</script>

<style>
.menuBar_item{
    position: relative;
    padding: 10px;
    display: inline;
    border-top-left-radius: 10px;
    border-top-right-radius: 10px;
}

.menuBar_item a {
    text-align: center;
    padding: 14px 16px;
    text-decoration: none;
    color: rgb(17, 27, 32);
    cursor: pointer;
}

.activeLink {
    background-color: rgb(108, 134, 118)
}

.menuBar {
    margin: auto;
    max-width: 300px;
    padding: 10px;
}

</style>