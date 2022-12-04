<template>
    <form @submit.prevent="onSubmit">
        <div class="profile extended">
            <h2>Kurti kategoriją</h2>
            <a class="errorMessage"></a>
            <table>
                <tr>
                    <th>
                        <p>Pavadinimas</p>
                    </th>
                    <td>
                        <labeled-input id="name" :required="true" />ą
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Aprašymas</p>
                    </th>
                    <td>
                        <textarea class="input" id="description" :required="false" />
                    </td>
                </tr>
            </table>
        </div>
        <submitButton text="Išsaugoti" />
    </form>
</template>

<script>
import labeledInput from '@/components/input/labeledInput.vue';
import submitButton from '@/components/input/submitButton.vue';

export default {
    components: {
        labeledInput,
        submitButton
    },
    emits: [ 'triggerModal' ],
    methods: {
        async onSubmit(submitEvent) {
            var errorMessage = document.getElementsByClassName("errorMessage")[0]
            errorMessage.text = ''

            var body = {
                "name": submitEvent.target.elements.name.value,
                "description": submitEvent.target.elements.description.value,
            }

            var response = await this.performRequest(`/categories`, "POST", body)

            if(response.success){
                location.pathname = `/categories`
            }
            else{
                errorMessage.text = response.body?.message || "Įvyko klaida"
            }
        }
    },
    screen: true
}
</script>

<style>
.align {
    margin: 10px;
    text-align: left;
    cursor: pointer
}

.profile.extended textarea {
    max-width: 300px;
    min-width: 300px;
    max-height: 300px;
}
</style>