import { createWebHistory, createRouter } from "vue-router";
import Messages from "../views/Messages.vue";
import Endpoints from "../views/Endpoints.vue";
import Settings from "../views/Settings.vue";

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
    {
        path: "/settings",
        name: "Settings",
        component: Settings,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;