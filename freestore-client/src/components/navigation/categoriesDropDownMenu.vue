<template>
    <a v-on:click="toggleMenu()">{{ text || "Prekės" }}</a>
    <div class="menu">
        <div v-show="this.opened" class="menu__list" :class="{ 'menu__list--animate': this.opened }">
            <ul>
                <li v-for="category in this.categories" :key="category.id" :class="'menu__list__item'">
                    <a :href="'/categories/' + category.id + '/items'">{{ category.name }}</a>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
            opened: false,
            categories: []
        }
    },
    props: {
        text: String
    },
    methods: {
        async toggleMenu() {
            if (this.categories.length === 0) {
                var response = await this.performRequest("/categories", "GET")
                if (response.success) {
                    this.categories = response.body
                }
            }
            this.opened = !this.opened
        },
    }
} 
</script>

<style>
.menu {
    z-index: 1000;
    position: absolute;
}

.menu__list {
    box-shadow: 0px 5px 10px 0px #00000040;
    margin: auto;
    border-radius: 10px;
    margin-top: 15px;
    background-color: #91a1a1;
    text-align: center;
    width: 250px;
}

.menu__list ul {
    padding: 3px;
}

.menu__list li {
    padding: 5px;
    display: block;
}

.menu__list li:hover {
    background-color: rgb(108, 134, 118);
    border-radius: 10px;
}

.menu__list__item {
    list-style: none;
    padding-bottom: 15px;
    top: 0px;
}

.menu__list__item a {
    position: block;
    text-decoration: none;
    text-align: center;
}

@keyframes opacityAnimation {
    0% {
        opacity: 0;
    }

    100% {
        opacity: 1;
    }
}


.menu__list--animate {
    animation-name: opacityAnimation;
    animation-duration: 0.3s;
}
</style>