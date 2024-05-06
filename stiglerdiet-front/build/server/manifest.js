const manifest = (() => {
function __memo(fn) {
	let value;
	return () => value ??= (value = fn());
}

return {
	appDir: "_app",
	appPath: "_app",
	assets: new Set(["favicon.png","robots.txt"]),
	mimeTypes: {".png":"image/png",".txt":"text/plain"},
	_: {
		client: {"start":"_app/immutable/entry/start.ylYp533O.js","app":"_app/immutable/entry/app.CGVq1Tah.js","imports":["_app/immutable/entry/start.ylYp533O.js","_app/immutable/chunks/entry.DeciWqjl.js","_app/immutable/chunks/scheduler.Bmg8oFKD.js","_app/immutable/chunks/index.D_GRTHN4.js","_app/immutable/entry/app.CGVq1Tah.js","_app/immutable/chunks/scheduler.Bmg8oFKD.js","_app/immutable/chunks/index.Dac_rt9t.js"],"stylesheets":[],"fonts":[],"uses_env_dynamic_public":false},
		nodes: [
			__memo(() => import('./chunks/0-LtLSHtna.js')),
			__memo(() => import('./chunks/1-B_H70jjv.js'))
		],
		routes: [
			
		],
		matchers: async () => {
			
			return {  };
		},
		server_assets: {}
	}
}
})();

const prerendered = new Set(["/","/ruuat"]);

const base = "";

export { base, manifest, prerendered };
//# sourceMappingURL=manifest.js.map
