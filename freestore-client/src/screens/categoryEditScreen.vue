<template>
  <form @submit.prevent="onSubmit">
        <div v-if="this.category" class="profile extended">
            <h2>Redaguoti kategoriją</h2>
            <a class="errorMessage"></a>
            <table>
                <tr>
                    <th>
                        <p>Pavadinimas</p>
                    </th>
                    <td>
                        <labeled-input id="name" :value="category.name" :required="true" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Aprašymas</p>
                    </th>
                    <td>
                        <textarea class="input" id="description" :value="category.description" :required="false" />
                    </td>
                </tr>
            </table>
        </div>
        <submitButton text="Išsaugoti pakeitimus" />
    </form>
</template>

<script>
import submitButton from '@/components/input/submitButton.vue'
import labeledInput from '@/components/input/labeledInput.vue'

export default {
    data(){
        return {
            category: null
        }
    },
    components:{
        submitButton,
        labeledInput
    },
    emits: [ 'triggerModal' ],
    async beforeMount(){
        var catId = location.pathname.split('/')[2]
        var response = await this.performRequest(`/categories/${catId}`, 'GET')
        if(response.success){
            this.category = response.body
        }

        if(response.statusCode === 401){
            this.lcation = '/login'
        }
    },
    methods:{
        async onSubmit(submitEvent){
            var errorMessage = document.getElementsByClassName("errorMessage")[0]
            errorMessage.text = ''
            var body = {
                "name": submitEvent.target.elements.name.value,
                "description": submitEvent.target.elements.description.value,
            }

            var response = await this.performRequest(`/categories/${this.category.id}`, 'PUT', body)
            if(response.success){
                location.pathname = '/categories'
            }
            else{
                errorMessage.text = response.body?.message || "Įvyko klaida"
            }
        }
    }

}
</script>

<style>

</style>