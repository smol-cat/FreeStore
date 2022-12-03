<template>
    <h1>Kategorijų valdymas</h1>
    <table v-if="this.categories" class="item usersList">
        <thead>
            <tr>
                <th>
                    <p>ID</p>
                </th>
                <th>
                    <p>Pavadinimas</p>
                </th>
                <th>
                    <p>Aprašymas</p>
                </th>
                <th></th>
                <th></th>
            </tr>
            <categoryItem v-for="category in this.categories" v-bind:key="category.id" :category="category" @triggerModal="(text, onConfirm) => this.$emit('triggerModal', text, onConfirm)" />
        </thead>
    </table>
    <HRef label="Kurti naują kategoriją" link="/categories/new" />
</template>

<script>
import categoryItem from '@/components/informational/categoryItem.vue'
import HRef from '@/components/navigation/hRef.vue'

export default {
    data() {
        return {
            categories: null
        }
    },
    components: {
        categoryItem,
        HRef
    },
    emits: ['triggerModal'],
    async beforeMount() {
        if (sessionStorage["userLevel"] === 0) {
            location.pathname = '/'
        }

        var request = await this.performRequest('/categories', 'GET')
        if (request.success) {
            this.categories = request.body
        }

    }
}
</script>

<style>

</style>