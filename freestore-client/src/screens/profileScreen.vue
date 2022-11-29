<template>
    <div v-if="this.loaded" class="profile">
        <table>
            <tr>
                <th>
                    <p>Vardas</p>
                </th>
                <td>
                    <p>{{ this.userData.name + " " + this.userData.last_name }}</p>
                </td>
            </tr>
            <tr>
                <th>
                    <p>El. paštas</p>
                </th>
                <td>
                    <p>{{ this.userData.email }}</p>
                </td>
            </tr>
            <tr>
                <th>
                    <p>Tel. numeris</p>
                </th>
                <td>
                    <p>{{ this.userData.phoneNumber || "nenustatyta" }}</p>
                </td>
            </tr>
            <tr>
                <th>
                    <p>Rolė</p>
                </th>
                <td>
                    <p>{{ this.userData.role }}</p>
                </td>
            </tr>
        </table>
    </div>
    <HRef label="Redaguoti paskyrą" link="/profile/edit" />
</template>

<script>
import HRef from '@/components/navigation/hRef.vue';

export default {
    data() {
        return {
            loaded: false,
            userData: {}
        }
    },
    components: { HRef },
    async beforeMount() {
        if (!this.loaded) {
            var response = await this.performRequest("/accounts/own", "GET")
            if (response.success) {
                this.userData = response.body
                this.loaded = true
            }
        }
    },
    methods: {
        onSubmit(){
            
        }
    }
}
</script>

<style>
.profile {
    border-radius: 10px;
    margin: auto;
    padding: 10px;
    margin-top: 30px;
    background-color: #91a1a1;
    max-width: 400px;
}

.profile table {
    margin: auto;
}

.profile th p {
    margin: 5px;
    text-align: right;
}

.profile td p {
    margin: 5px;
    text-align: left;
}
</style>