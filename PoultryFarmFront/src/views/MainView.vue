<template>
  <BaseLayout>
    <div v-if="!loading">
      <transition name="fade" mode="out-in">
        <form v-if="creation" @submit.prevent="storeChicken()" class="form-create">
          <input v-model="chickenDTO.weight" type="number" step="0.1" placeholder="Вес" required>
          <input v-model="chickenDTO.age" type="number" step="0.1" placeholder="Возраст" required>
          <input v-model="chickenDTO.eggsPerMonth" type="number" step="0.1" placeholder="Яйценосонсть" required>
          <select v-model="chickenDTO.breedId" required>
            <option value=""></option>
            <option v-for="breed in breeds" :key="breed.id" :value="breed.id">
              {{ breed.name }}
            </option>
          </select>
          <select v-model="chickenDTO.cageId" required>
            <option value=""></option>
            <option v-for="cage in freeCages" :key="cage.id" :value="cage.id">
              {{ cage.id }}
            </option>
          </select>
          <div>
            <button class="btn-primary" type="submit"> Добавить </button>
          </div>
          <div class="form-create__errors" v-if="errorCreation">
            <ul>
              <li v-for="(error, index) in errorCreation" :key="index">
                {{ error }}
              </li>
            </ul>
          </div>
        </form>
      </transition>
      <div class="chickens">
        <div class="chickens-action">
          <button v-if="!creation" class="btn-primary" @click="creation = true"> Добавить </button>
          <button v-else class="btn-secondary" @click="creation = false"> Отмена </button>
        </div>
        <div class="chickens-item" v-for="chicken in chickens" :key="chicken.id">
          <slot v-if="!chicken.editing">
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Номер курицы </span> {{ chicken.id }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Вес </span> {{ chicken.weight }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Возраст </span> {{ chicken.age }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Яиц за месяц </span> {{ chicken.eggsPerMonth }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Порода </span> {{ chicken.breed.name }}
            </div>
            <div class="chickens-item__character">
              <span class="chickens-item__character-label"> Клетка </span> {{ chicken.cage.id }}
            </div>
            <button class="btn-secondary" @click="toggleEdit(chicken)"> Редактировать </button>
          </slot>
          <form v-else @submit.prevent="updateChicken(chicken)" class="form-update">
            <div class="form-update__title"></div>Редактирование курицы {{chicken.id}}
            <input v-model="chicken.weight" type="number" step="0.1" placeholder="Вес" required>
            <input v-model="chicken.age" type="number" step="0.1" placeholder="Возраст" required>
            <input v-model="chicken.eggsPerMonth" type="number" step="0.1" placeholder="Яйценосонсть" required>
            <select v-model="chicken.breedId" required>
              <option v-for="breed in breeds" :key="breed.id" :value="breed.id">
                {{ breed.name }}
              </option>
            </select>
            <select v-model="chicken.cageId" required>
              <option :value="chicken.cage.id">{{chicken.cage.id}}</option>
              <option v-for="cage in freeCages" :key="cage.id" :value="cage.id">
                {{ cage.id }}
              </option>
            </select>
            <div class="form-update__buttons">
              <button class="btn-primary" type="submit"> Сохранить </button>
              <button class="btn-secondary" type="button" @click="toggleEdit(chicken)"> Отмена </button>
              <button class="btn-danger" type="button" @click.prevent="deleteChicken(chicken)"> Удалить </button>
            </div>
            <div class="form-update__errors" v-if="errorUpdating">
              <ul>
                <li v-for="(error, index) in errorUpdating" :key="index">
                  {{ error }}
                </li>
              </ul>
            </div>
          </form>
        </div>
      </div>
    </div>
    <div v-else>
      Loading...
    </div>
  </BaseLayout>
</template>

<script>
import BaseLayout from '@/layouts/BaseLayout.vue';
import axios from 'axios';
import Swal from "sweetalert2";

export default {
  name: 'MainView',
  components: {
    BaseLayout
  },
  data() {
    return {
      loading: true,
      creation: false,
      chickens: [],
      breeds: [],
      cages: [],
      freeCages: [],
      chickenDTO: {
        weight: '',
        age: '',
        eggsPerMonth: '',
        breedId: '',
        cageId: '',
      },
      errorCreation: [],
      errorUpdating: [],
    };
  },
  methods: {
    getchickens() {
      this.loading = true;
      axios.get('http://localhost:8080/api/chicken')
          .then(res => {
            this.chickens = res.data.map(chicken => ({
              ...chicken,
              editing: false
            }));
          })
          .catch(err => {
            console.log(err);
          })
          .finally(() => {
            this.loading = false;
          });
    },
    getBreeds() {
      axios.get('http://localhost:8080/api/breed')
          .then(res => {
            this.breeds = res.data;
          })
          .catch(err => {
            console.log(err);
          });
    },
    getCages() {
      axios.get('http://localhost:8080/api/cage')
          .then(res => {
            this.cages = res.data;
            this.freeCages = this.cages.filter(cage => !cage.isOccupied);
          })
          .catch(err => {
            console.log(err);
          });
    },
    storeChicken() {
      this.errorCreation = [];
      const chicken = {
        weight: Number(this.chickenDTO.weight),
        age: Number(this.chickenDTO.age),
        eggsPerMonth: Number(this.chickenDTO.eggsPerMonth),
        breedId: Number(this.chickenDTO.breedId),
        cageId: Number(this.chickenDTO.cageId)
      };
      axios.post('http://localhost:8080/api/chicken', chicken)
          .then(res => {
            this.getchickens();
          })
          .catch(err => {
            this.errorCreation = err.response.data.errors;
            console.log(this.errorCreation);
          });
    },
    toggleEdit(chicken) {
      chicken.editing = !chicken.editing;
    },
    updateChicken(chicken) {
      this.errorsUpdating = [];
      const chickenDTO = {
        id: Number(chicken.id),
        weight: Number(chicken.weight),
        age: Number(chicken.age),
        eggsPerMonth: Number(chicken.eggsPerMonth),
        breedId: Number(chicken.breedId),
        cageId: Number(chicken.cageId)
      };
      axios.put(`http://localhost:8080/api/chicken/${chicken.id}`, chickenDTO)
          .then(res => {
            console.log(res.data);
            this.getchickens();
            this.getCages();
            chicken.editing = false;
          })
          .catch(err => {
            this.errorUpdating = err.response?.data?.errors || "Ошибка при обновлении";
            console.log(this.errorUpdating);
          });
    },
    deleteChicken(chicken) {
      Swal.fire({
        title: 'Вы уверены?',
        text: 'Это действие нельзя отменить!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Да, удалить!',
        cancelButtonText: 'Отмена'
      }).then((result) => {
        if (result.isConfirmed) {
          axios.delete(`http://localhost:8080/api/chicken/${chicken.id}`)
              .then(() => {
                Swal.fire('Удалено!', 'Курица успешно удалена.', 'success');
                this.getchickens();
                this.getCages();
              })
              .catch(err => {
                Swal.fire('Ошибка!', 'Не удалось удалить курицу.', 'error');
                console.log(err);
              });
        }
      });
    }
  },
  mounted() {
    this.getchickens();
    this.getBreeds();
    this.getCages();
  }
};
</script>


<style lang="scss" scoped>
.form-create {
  display: flex;
  flex-direction: column;
  gap: 10px;

  margin-bottom: 25px;

  &__errors {
    color: #910303;
    font-weight: 600;
  }
}

.chickens {
  display: flex;
  flex-direction: column;
  gap: 15px;

  &-item{
    width: 100%;

    padding: 10px;
    border-radius: 10px;
    border: 1px solid #000;

    display: flex;
    flex-direction: row;
    gap: 5px;

    &__character{
      display: flex;
      flex-direction: column;
      gap: 5px;

      width: 15%;

      font-size: 14px;

      &-label{
        font-size: 10px !important;
        color: gray;
      }
    }

    .form-update{
      width: 100%;
      display: flex;
      flex-direction: column;
      gap: 10px;

      margin-bottom: 25px;

      &__errors {
        padding-left: 25px;
        color: #910303;
        font-weight: 600;
      }

      &__buttons{
        display: flex;
        flex-direction: row;
        gap: 5px;
      }
    }
  }
}
</style>
  