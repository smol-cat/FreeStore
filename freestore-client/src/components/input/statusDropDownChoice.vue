<template>
    <a v-on:click="toggleMenu()" :class="{chosenCategory: chosen}">{{ chosen || text }}</a>
    <div class="menu">
        <div v-show="this.opened" class="menu__list" :class="{ 'menu__list--animate': this.opened }">
            <ul>
                <li v-for="status in this.statuses" :key="status.id" :class="'menu__list__item'">
                    <a v-on:click="choose(status)">{{ status.name }}</a>
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
            chosen: null,
            statuses: [
                { "id": 1, name: "Parduodama" },
                { "id": 2, name: "Rezervuota" },
                { "id": 3, name: "Parduota" }
            ]
        }
    },
    beforeMount(){
        if(this.chosenStatus){
            this.chosen = this.chosenStatus
        }
    },
    props: {
        chosenStatus: String,
        text: String
    },
    emits: ['chooseStatus'],
    methods: {
        async toggleMenu() {
            this.opened = !this.opened
        },
        choose(status) {
            this.chosen = status.name
            this.$emit('chooseStatus', status)
            this.toggleMenu()
        }
    }
} 
</script>

<style>
</style>