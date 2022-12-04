<template>
    <div class="menuWrapper">
        <i class="menuBar_item fa fa-bars icon" :class="{ activeLink: this.responsive }"
            v-on:click="toggleResponsiveMenu()" />
        <ul class="menuBar" :class="{ responsive: this.responsive }">
            <li v-for="(link, name) in getElements()" :key="name" class="menuBar_item"
                :class="{ activeLink: link === this.getPath() }">
                <a :href="link">{{ name }}</a>
            </li>
            <li class="menuBar_item">
                <categories-drop-down-menu />
            </li>
        </ul>
    </div>
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
            },
            responsive: false
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
        },
        toggleResponsiveMenu() {
            this.responsive = !this.responsive
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

.menuBar ul {
    margin: auto;
    padding: 10px;
    padding-bottom: 0;
}

.menuBar {
    padding: 0;
}

.icon.fa {
    margin: auto;
    width: 200px;
    color: black;
    cursor: pointer;
    display: none;
}

@media screen and (max-width: 768px) {
    .menuBar li {
        display: none;
    }

    .icon.fa {
        display: inline-block;
    }

}

.menuBar.fa {
    position: relative
}


@media screen and (max-width: 768px) {
    .menuBar.responsive {
        z-index: 1000;
        position: absolute;
        background-color: #91a1a1;
        box-shadow: 0px 5px 10px 0px #00000040;
        margin: 0% auto;
        border-radius: 10px;
        margin-top: 15px;
        width: 220px;
    }

    .menuBar.responsive li {
        float: none;
        display: block;
        text-align: center;
        border-radius: 10px;
    }

    .menuWrapper{
        margin: auto;
        width: 220px;
    }

}
</style>