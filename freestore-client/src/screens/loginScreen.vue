<template>
    <form class="loginForm" @submit.prevent="onSubmit">
        <a class="errorMessage"></a>
        <fieldset>
            <labeledInput id="email" type="text" placeholder="El.paštas" required="true" />
            <labeledInput id="password" type="password" placeholder="Slaptažodis" required="true" />
        </fieldset>
        <submitButton text="Prisijungti" />
        <hRef :label="'Neturite paskyros? Registruokitės'" :link="'/register'" />
    </form>
</template>

<script>
import labeledInput from '../components/input/labeledInput.vue'
import submitButton from '../components/input/submitButton.vue'
import hRef from '@/components/navigation/hRef.vue';

export default {
    name: 'loginScreen',
    components: {
        labeledInput, 
        submitButton,
        hRef
    },
    emits: [ 'triggerModal' ],
    methods: {
        async onSubmit(submitEvent) {
            var body = {
                "email": submitEvent.target.elements.email.value,
                "password": submitEvent.target.elements.password.value
            }
            var response = await this.performRequest("/tokens", "POST", body)
            if(response.success){
                document.getElementsByClassName("errorMessage")[0].text = ''
                localStorage.token = response.body.token
                location.pathname = ""
            }
            else{
                document.getElementsByClassName("errorMessage")[0].text = response.body?.message || "Įvyko klaida"
            }
        }
    }
}
</script>

<style>
.loginForm {
    margin-top: 30px;
    margin-left: auto;
    margin-right: auto;
    position: relative;
    display: block;
    box-sizing: content-box;
    width: 400px
}

fieldset {
    border: 0;
}

.errorMessage{
    color: red;
}
</style>
