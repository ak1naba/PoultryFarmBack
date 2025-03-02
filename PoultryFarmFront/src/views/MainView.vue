<template>
    <BaseLayout>
      <div v-if="!loading">
        <transition name="fade" mode="out-in">
          <form v-if="creation" @submit.prevent="storeChicken()" class="form-create">
            <input v-model="chickenDTO.weight" type="number" placeholder="Вес" required>
            <input v-model="chickenDTO.age" type="number" placeholder="Возраст" required>
            <input v-model="chickenDTO.eggsPerMonth" type="number" placeholder="Яйценосонсть" required>
            <select v-model="chickenDTO.breedId" required>
              <option value=""></option>
              <option v-for="breed in breeds" :key="breed.id" :value="breed.id">
                {{breed.name}}
              </option>
            </select>
            <select v-model="chickenDTO.cageId" required>
              <option value=""></option>
              <option value="1">1</option>
              <option v-for="cage in freeCages" :key="cage.id" :value="cage.id">
                {{cage.id}}
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

        chickenDTO:{
          weight: '',
          age: '',
          eggsPerMonth: '',
          breedId: '',
          cageId: '',
        },

        errorCreation: [],
      };
    },
    methods: {
      getchickens() {
        this.loading = true;
        axios.get('http://localhost:8080/api/chicken')
            .then(res => {
              this.chickens = res.data;
            })
            .catch(err => {
              console.log(err);
            })
            .finally(() => {
              this.loading = false;
            })
      },
      getBreeds() {
        axios.get('http://localhost:8080/api/breed')
        .then(res => {
          this.breeds = res.data;
          this.breeds.push({id:4, name: 'fake', description: ''});
        })
        .catch(err => {
          console.log(err);
        })
      },
      getCages() {
        axios.get('http://localhost:8080/api/cage')
        .then(res => {
          this.cages = res.data;

          this.cages.forEach((cage) => {
            if (!cage.isOccupied)
              this.freeCages.push(cage);
          });
        })
        .catch(err => {
          console.log(err);
        })
      },

      storeChicken() {
        this.errorCreation = [];

        const chicken = {
          "weight":  Number(this.chickenDTO.weight),
          "age":  Number(this.chickenDTO.age),
          "eggsPerMonth":  Number(this.chickenDTO.eggsPerMonth),
          "breedId": Number(this.chickenDTO.breedId),
          "cageId": Number(this.chickenDTO.cageId)
        };

        axios.post('http://localhost:8080/api/chicken', chicken)
        .then(res => {
          this.getchickens();
        })
        .catch(err => {
          this.errorCreation = err.response.data.errors;
          console.log(this.errorCreation);
        })
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
  }
}
</style>
  