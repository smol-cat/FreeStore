<template>
    <form @submit.prevent="onSubmit">
        <div class="profile extended">
            <h2>Kurti skelbimą</h2>
            <a class="errorMessage"></a>
            <table>
                <tr>
                    <th>
                        <p>Pavadinimas</p>
                    </th>
                    <td>
                        <labeled-input id="title" :required="true" />
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
                <tr>
                    <th>
                        <p>Kaina</p>
                    </th>
                    <td>
                        <labeled-input id="price" :_type="'number'" :required="true" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <p>Kategorija</p>
                    </th>
                    <td>
                        <div class="align">
                            <categories-drop-down-choice :text="'Pasirinkti'"
                                @chooseCategory="(cat) => this.chosenCategory = cat" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <submitButton text="Skelbti" />
    </form>
</template>

<script>
import labeledInput from '@/components/input/labeledInput.vue';
import categoriesDropDownChoice from '@/components/input/categoriesDropDownChoice.vue';
import submitButton from '@/components/input/submitButton.vue';

export default {
    data() {
        return {
            chosenCategory: null
        }
    },
    components: {
        labeledInput,
        categoriesDropDownChoice,
        submitButton
    },
    emits: [ 'triggerModal' ],
    methods: {
        async onSubmit(submitEvent) {
            var errorMessage = document.getElementsByClassName("errorMessage")[0]
            errorMessage.text = ''

            if(!this.chosenCategory){
                errorMessage.text = "Pasirinkite kategoriją"
                return
            }

            var body = {
                "title": submitEvent.target.elements.title.value,
                "description": submitEvent.target.elements.description.value,
                "price": submitEvent.target.elements.price.value
            }

            var response = await this.performRequest(`/categories/${this.chosenCategory.id}/items`, "POST", body)

            if(response.success){
                location.pathname = `/categories/${this.chosenCategory.id}/items`
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