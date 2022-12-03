<template>
    <h1 v-if="category">{{ this.category.name }}</h1>
    <div v-if="this.items">
        <postItem v-for="item in this.items" :key="item.id" :item="item" :categoryId="this.category.id"/>
    </div>
</template>

<script>
import postItem from '@/components/informational/postItem.vue'

export default {
    data() {
        return {
            category: null,
            items: null,
        }
    },
    components:{
        postItem
    },
    async beforeMount() {
        var categoryId = window.location.pathname.split('/')[2]
        this.loadCategory(categoryId)
        this.loadItems(categoryId)
    },
    emits: ['triggerModal'],
    methods: {
        async loadCategory(catId) {
            var response = await this.performRequest(`/categories/${catId}`)
            if (response.success) {
                this.category = response.body
            }

        },
        async loadItems(catId) {
            var response = await this.performRequest(`/categories/${catId}/items`, "GET")
            if (response.statusCode === 401) {
                location.pathname = "/login"
            }

            if(response.success){
                this.items = response.body
            }
        }
    },
    screen: true
}
</script>

<style>

</style>