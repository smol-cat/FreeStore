<template>
    <form @submit.prevent="onSubmit">
        <div v-if="this.item" class="profile extended">
            <h2>Redaguoti skelbimą</h2>
            <a class="errorMessage"></a>
            <table>
                <tr>
                    <th>
                        <p>Pavadinimas</p>
                    </th>
                    <td>
                        <labeled-input id="title" :_value="this.item.title" :required="true" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Aprašymas</p>
                    </th>
                    <td>
                        <textarea class="input" id="description" :value="this.item.description" :required="false" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Kaina</p>
                    </th>
                    <td>
                        <labeled-input id="price" :_type="'number'" :_value="this.item.price.toString()" :required="true" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Kategorija</p>
                    </th>
                    <td>
                        <div class="align">
                            <categories-drop-down-choice :text="'Pasirinkti'" :chosenCategory="this.chosenCategory.name" @chooseCategory="(cat) => this.chosenCategory = cat" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Būsena</p>
                    </th>
                    <td>
                        <div class="align">
                            <status-drop-down-choice :text="'Pasirinkti'" :chosenStatus="this.chosenStatus.name" @chooseStatus="(status) => this.chosenStatus = status" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <submitButton text="Išsaugoti" />
    </form>
</template>

<script>
import labeledInput from '@/components/input/labeledInput.vue';
import categoriesDropDownChoice from '@/components/input/categoriesDropDownChoice.vue';
import statusDropDownChoice from '@/components/input/statusDropDownChoice.vue';
import submitButton from '@/components/input/submitButton.vue';


export default {
    data() {
        return {
            item: null,
            chosenCategory: null,
            chosenStatus: null
        }
    },
    components: {
        labeledInput,
        categoriesDropDownChoice,
        submitButton,
        statusDropDownChoice
    },
    emits: [ 'triggerModal' ],
    async beforeMount(){
        var endpoints = location.pathname.split('/')
        var response = await this.performRequest(`/categories/${endpoints[2]}/items/${endpoints[4]}`, 'GET')
        if(response.success){
            this.item = response.body
            this.chosenCategory = response.body.category
            this.chosenStatus = response.body.state
        }
    },
    methods: {
        async onSubmit(submitEvent) {
            var errorMessage = document.getElementsByClassName("errorMessage")[0]
            errorMessage.text = ''

            if(!this.chosenCategory){
                errorMessage.text = "Pasirinkite kategoriją"
                return
            }

            if(!this.chosenStatus){
                errorMessage.text = "Pasirinkite būseną"
                return
            }

            var body = {
                "title": submitEvent.target.elements.title.value,
                "description": submitEvent.target.elements.description.value,
                "price": submitEvent.target.elements.price.value,
                "category_id": this.chosenCategory.id,
                "status_id": this.chosenStatus.id,
            }

            var response = await this.performRequest(`/categories/${this.item.category.id}/items/${this.item.id}`, "PUT", body)

            if(response.success){
                location.pathname = `/categories/${this.chosenCategory.id}/items/${this.item.id}`
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