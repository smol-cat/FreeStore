<template>
    <tr>
        <td>
            <p>{{ category.id }}</p>
        </td>
        <td>
            <p>{{ category.name }}</p>
        </td>
        <td>
            <p>{{ category.description }}</p>
        </td>
        <td>
            <block-button :onClick="() => this.toEdit(category.id)" :text="'redaguoti'" />
        </td>
        <td>
            <block-button
                :onClick="() => this.$emit('triggerModal', 'Ar norite pašalinti šią kategoriją?', async () => await this.removeCategory(category.id))"
                :text="'naikinti'" />
        </td>
    </tr>
</template>

<script>
import blockButton from '../input/blockButton.vue';

export default {
    components: {
        blockButton
    },
    props: {
        category: Object
    },
    emits: ['triggerModal'],
    methods: {
        toEdit(id) {
            window.location.pathname = `/categories/${id}/edit`
        },
        async removeCategory(id) {
            var response = await this.performRequest(`/categories/${id}`, "DELETE")
            if (response.success) {
                location.reload()
            }
            else {
                console.log('fuck you')
            }
        }
    }
}
</script>

<style>

</style>