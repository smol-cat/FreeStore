<template>
    <tr>
        <td>
            <p>{{ this.user.id }}</p>
        </td>
        <td>
            <p>{{ `${this.user.name} ${this.user.last_name}` }}</p>
        </td>
        <td>
            <p>{{ user.role }}</p>
        </td>
        <td>
            <p>{{ this.getDate(user.date_created) }}</p>
        </td>
        <td>
            <block-button :onClick="() => blockUser(user)" :text="'blokuoti'"/>
        </td>
    </tr>
</template>

<script>
import blockButton from '../input/blockButton.vue'
export default {
    components:{
        blockButton
    },
    props: {
        user: Object
    },
    emits: [ 'blockUser' ],
    methods: {
        getDate(dateString) {
            var date = new Date(dateString)
            return Intl.DateTimeFormat('lt', {
                dateStyle: "short"
            }).format(date)
        },
        blockUser(user){
            this.$emit("blockUser", user)
        }
    }
}
</script>

<style>

</style>