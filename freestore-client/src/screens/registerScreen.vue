<template>
    <form class="loginForm" @submit.prevent="onSubmit">
        <a class="errorMessage"></a>
        <a class="successMessage"></a>
        <labeledInput :id="'name'" :_placeholder="'Vardas'" :_required="true" :_type="'text'" />
        <labeledInput :id="'lastName'" :_placeholder="'Pavardė'" :_required="true" :_type="'text'" />
        <labeledInput :id="'email'" :_placeholder="'El. paštas'" :_required="true" :_type="'text'" />
        <labeledInput :id="'password'" :_placeholder="'Slaptažodis'" :_required="true" :_type="'password'" />
        <labeledInput :id="'passwordRepeat'" :_placeholder="'Pakartoti slaptažodį'" :_required="true" :_type="'password'" />
        <submitButton :text="'Registruotis'" />
        <hRef :label="'Turite paskyrą? Prisijunkite'" :link="'/login'" />
    </form>
</template>


<script>
import labeledInput from '@/components/input/labeledInput.vue';
import submitButton from '@/components/input/submitButton.vue';
import hRef from '@/components/navigation/hRef.vue';

export default {
    components: {
        labeledInput,
        submitButton,
        hRef
    },
    emits: [ 'triggerModal' ],
    methods: {
        async onSubmit(submitEvent) {
            var errorMessageField = document.getElementsByClassName("errorMessage")[0]
            var successMessageField = document.getElementsByClassName("successMessage")[0]
            errorMessageField.text = ""
            successMessageField.text = ""
            var formElements = submitEvent.target.elements
            if (formElements.password.value !== formElements.passwordRepeat.value) {
                errorMessageField.text = "Slaptažodžiai nesutampa"
                return
            }

            var body = {
                "name": formElements.name.value,
                "lastName": formElements.lastName.value,
                "email": formElements.email.value,
                "password": formElements.password.value
            }

            var response = await this.performRequest("/accounts", "POST", body)
            if (response.success) {
                successMessageField.text = "Sėkmingai prisiregistravote, galite prisijungti"
            }
            else {
                errorMessageField.text = response.body?.message || "Įvyko klaida"
            }
        }
    }
}
</script>

<style>
.successMessage {
    color: green
}
</style>