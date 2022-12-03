<template>
    <form @submit.prevent="onSubmit">
        <div class="profile extended">
            <h2>Profilio redagavimas</h2>
            <a class="errorMessage"></a>
            <a class="successMessage"></a>
            <table>
                <tr>
                    <th>
                        <p>Vardas</p>
                    </th>
                    <td>
                        <labeled-input id="name" :value="this.userData.name" :required="true" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Pavardė</p>
                    </th>
                    <td>
                        <labeled-input id="lastName" :value="this.userData.last_name" :required="true" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Tel. numeris</p>
                    </th>
                    <td>
                        <labeled-input id="phoneNumber" :value="this.userData.phoneNumber" :required="false" />
                    </td>
                </tr>
            </table>
        </div>
        <submitButton text="Išsaugoti pakeitimus" />
    </form>
</template>

<script>
import labeledInput from '@/components/input/labeledInput.vue'
import submitButton from '@/components/input/submitButton.vue'

export default {
    components: {
        labeledInput,
        submitButton
    },
    data() {
        return {
            loaded: false,
            userData: {}
        }
    },
    async beforeMount() {
        if (!this.loaded) {
            var response = await this.performRequest("/accounts/own", "GET")
            if (response.success) {
                this.userData = response.body
                this.loaded = true
            }
        }
    },
    emits: [ 'triggerModal' ],
    methods: {
        async onSubmit(submitEvent) {
            var errorMessage = document.getElementsByClassName("errorMessage")[0]
            var successMessage = document.getElementsByClassName("successMessage")[0]
            errorMessage.text = ''
            successMessage.text = ''
            var body = {
                "name": submitEvent.target.elements.name.value,
                "lastName": submitEvent.target.elements.lastName.value,
                "phoneNumber": submitEvent.target.elements.phoneNumber.value
            }

            var response = await this.performRequest("/accounts", "PUT", body)
            if (response.success) {
                successMessage.text = 'Profilis atnaujintas'
            }
            else {
                errorMessage.text = response.body?.message || "Įvyko klaida"
            }
        }
    }
}
</script>

<style>
.extended {
    max-width: 500px;
}
</style>