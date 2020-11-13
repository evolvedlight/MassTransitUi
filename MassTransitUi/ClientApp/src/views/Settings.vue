<template>
  <div>
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
      <h1 class="h2">Settings</h1>
      <div class="btn-toolbar mb-2 mb-md-0">
        <div class="btn-group mr-2">
          <button type="button" class="btn btn-sm btn-outline-secondary">Import/Export</button>
        </div>
      </div>
    </div>
    <div class="my-2">
      <div class="card">
        <div class="card-header">
          Config
        </div>
        <div class="card-body">
          <form>
            <div class="form-check">
              <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault">
              <label class="form-check-label" for="flexCheckDefault">
                Automatically check queues based on MassTransit Default "_error" queues
              </label>
            </div>
          </form>
        </div>
      </div>
    </div>
    <div class="my-2">
      <div class="card">
        <div class="card-header">
          RabbitMq Server
        </div>
        <div class="card-body">
          <table class="table table-borderless">
            <tbody>
              <tr>
                <td>Server Address</td>
                <td>ampq://xxx</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Settings',
  props: {
    endpoints: Array,
  },
  methods: {
    
  }
}
</script>
