<template>
  <div>
    <header class="navbar navbar-dark sticky-top bg-dark flex-md-nowrap p-0 shadow">
      <router-link to="/" class="navbar-brand col-md-3 col-lg-2 mr-0 px-3">Mass Transit UI</router-link>
      <button class="navbar-toggler position-absolute d-md-none collapsed" type="button" data-toggle="collapse" data-target="#sidebarMenu" aria-controls="sidebarMenu" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <input class="form-control form-control-dark w-100" type="text" v-model="quickFilter" placeholder="Search" aria-label="Search">
      <ul class="navbar-nav px-3">
        <li class="nav-item text-nowrap">
          <router-link to="/settings" class="nav-link">Settings</router-link>
        </li>
      </ul>
      <ul class="navbar-nav px-3">
        <li class="nav-item text-nowrap">
          <a class="nav-link" href="#">Sign out</a>
        </li>
      </ul>
    </header>
    <div class="container-fluid">
      <div class="row">
        <nav id="sidebarMenu" class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse">
          <div class="position-sticky pt-3">
            <ul class="nav flex-column">
              <li class="nav-item">
                <router-link to="/" class="nav-link"><span data-feather="file"></span>Failed Messages</router-link>
              </li>
              <li class="nav-item">
                <router-link to="/endpoints" class="nav-link"><span data-feather="shopping-cart"></span>Endpoints</router-link>
              </li>
            </ul>
          </div>
        </nav>

        <main class="col-md-9 ml-sm-auto col-lg-10 px-md-4">
          <router-view :quickFilter="quickFilter" /> 
        </main>
      </div>
    </div>
    
  </div>
  
</template>

<script>

export default {
  name: 'App',
  components: {
    
  },
  data() {
    return {
      quickFilter: '',
    }
  }
}
</script>
