import { createWebHistory, createRouter } from "vue-router";
import Messages from "../views/Messages.vue";
import Endpoints from "../views/Endpoints.vue";

const routes = [{
        path: "/",
        name: "Messages",
        component: Messages,
    },
    {
        path: "/endpoints",
        name: "Endpoints",
        component: Endpoints,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;