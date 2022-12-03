<template>
    <h1>Naudotojai</h1>
    <table v-if="users" class="item usersList">
        <thead>
            <tr>
                <th>
                    <p>ID</p>
                </th>
                <th>
                    <p>Vardas</p>
                </th>
                <th>
                    <p>Rolė</p>
                </th>
                <th>
                    <p>Sukūrimo data</p>
                </th>
                <th>
                </th>
            </tr>
        </thead>
        <user-item v-for="user in this.users" v-bind:key="user.id" :user="user" @blockUser="(user) => this.blockUser(user)" />
    </table>
</template>

<script>
import userItem from '@/components/informational/userItem.vue';

export default {
    data() {
        return {
            users: null
        }
    },
    components: {
        userItem
    },
    emits: ["triggerModal"],
    async beforeMount() {
        if (this.users) {
            return
        }

        var response = await this.performRequest('/accounts', 'GET')
        if (!response.success) {
            location.pathname = '/'
        }
        else {
            this.users = response.body
        }
    },
    methods:{
        blockUser(user){
            this.$emit("triggerModal", "Ar norite užblokuoti šį vartotoją?", async () => { 
                var request = await this.performRequest(`/accounts/${user.id}`, 'DELETE')
                if(request.success){
                    location.reload()
                }
            })
        }
    },
    screen: true
}
</script>

<style>

.item.usersList th {
    border-bottom: 2px solid rgb(17, 27, 32);
}

.item.usersList td{
    padding-right: 20px;
    white-space: nowrap;
}

.usersList th p {
    color: rgb(17, 27, 32);
}
</style>