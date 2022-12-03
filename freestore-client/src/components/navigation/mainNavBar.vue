<template>
    <ul class="menuBar">
        <li v-for="(link, name) in getElements()" :key="name" class="menuBar_item"
            :class="{ activeLink: link === this.getPath() }">
            <a :href="link">{{ name }}</a>
        </li>
        <li class="menuBar_item">
            <categories-drop-down-menu />
        </li>
    </ul>
</template>

<script>
import categoriesDropDownMenu from './categoriesDropDownMenu.vue'

export default {
    components: { categoriesDropDownMenu },
    data() {
        return {
            elements: {
                "Namai": "/"
            },
            userElements: {
                "Skelbti": "/newItem",
                "Profilis": "/profile",
            },
            adminElements: {
                "Kategorijos": "/categories",
                "Naudotojai": "/accounts"
            }
        }
    },
    props: {
        authenticated: Boolean,
        level: Number,
    },
    beforeMount() {
        this.elements = {
            "Namai": "/"
        }
    },
    methods: {
        getPath() {
            return window.location.pathname
        },
        getElements() {
            var elems = this.elements
            if (this.authenticated) {
                for (var elem in this.userElements) {
                    elems[elem] = this.userElements[elem]
                }
            }

            if (this.level > 0) {
                for (elem in this.adminElements) {
                    elems[elem] = this.adminElements[elem]
                }
            }
            return elems
        }
    },
    name: "mainNavBar",
}
</script>

<style>
.menuBar_item {
    position: relative;
    padding: 10px;
    display: inline-block;
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
    padding: 10px;
    padding-bottom: 0;
}
</style>