!function(n,l){"object"==typeof exports&&"undefined"!=typeof module?l(exports):"function"==typeof define&&define.amd?define(["exports"],l):l((n||self).preact={})}(this,function(n){var l,u,t,i,o,r,f,e,c,s=65536,a=1<<17,h={},p=[],v=/acit|ex(?:s|g|n|p|$)|rph|grid|ows|mnc|ntw|ine[ch]|zoo|^ord|itera/i,y=Array.isArray;function d(n,l){for(var u in l)n[u]=l[u];return n}function _(n){var l=n.parentNode;l&&l.removeChild(n)}function b(n,u,t){var i,o,r,f={};for(r in u)"key"==r?i=u[r]:"ref"==r?o=u[r]:f[r]=u[r];if(arguments.length>2&&(f.children=arguments.length>3?l.call(arguments,2):t),"function"==typeof n&&null!=n.defaultProps)for(r in n.defaultProps)void 0===f[r]&&(f[r]=n.defaultProps[r]);return g(n,f,i,o,null)}function g(n,l,i,o,r){var f={type:n,props:l,key:i,ref:o,__k:null,__:null,__b:0,__e:null,__d:void 0,__c:null,constructor:void 0,__v:null==r?++t:r,__i:-1,__u:0};return null==r&&null!=u.vnode&&u.vnode(f),f}function m(n){return n.children}function k(n,l){this.props=n,this.context=l}function w(n,l){if(null==l)return n.__?w(n.__,n.__i+1):null;for(var u;l<n.__k.length;l++)if(null!=(u=n.__k[l])&&null!=u.__e)return u.__e;return"function"==typeof n.type?w(n):null}function x(n){var l,u;if(null!=(n=n.__)&&null!=n.__c){for(n.__e=n.__c.base=null,l=0;l<n.__k.length;l++)if(null!=(u=n.__k[l])&&null!=u.__e){n.__e=n.__c.base=u.__e;break}return x(n)}}function P(n){(!n.__d&&(n.__d=!0)&&o.push(n)&&!S.__r++||r!==u.debounceRendering)&&((r=u.debounceRendering)||f)(S)}function S(){var n,l,t,i,r,f,c,s,a;for(o.sort(e);n=o.shift();)n.__d&&(l=o.length,i=void 0,f=(r=(t=n).__v).__e,s=[],a=[],(c=t.__P)&&((i=d({},r)).__v=r.__v+1,u.vnode&&u.vnode(i),L(c,i,r,t.__n,void 0!==c.ownerSVGElement,32&r.__u?[f]:null,s,null==f?w(r):f,!!(32&r.__u),a),i.__.__k[i.__i]=i,M(s,i,a),i.__e!=f&&x(i)),o.length>l&&o.sort(e));S.__r=0}function T(n,l,u,t,i,o,r,f,e,c,a){var v,y,d,_,b,g=t&&t.__k||p,m=l.length;for(u.__d=e,$(u,l,g),e=u.__d,v=0;v<m;v++)null!=(d=u.__k[v])&&"boolean"!=typeof d&&"function"!=typeof d&&(y=-1===d.__i?h:g[d.__i]||h,d.__i=v,L(n,d,y,i,o,r,f,e,c,a),_=d.__e,d.ref&&y.ref!=d.ref&&(y.ref&&N(y.ref,null,d),a.push(d.ref,d.__c||_,d)),null==b&&null!=_&&(b=_),d.__u&s||y.__k===d.__k?e=C(d,e,n):"function"==typeof d.type&&void 0!==d.__d?e=d.__d:_&&(e=_.nextSibling),d.__d=void 0,d.__u&=-196609);u.__d=e,u.__e=b}function $(n,l,u){var t,i,o,r,f,e=l.length,c=u.length,h=c,p=0;for(n.__k=[],t=0;t<e;t++)null!=(i=n.__k[t]=null==(i=l[t])||"boolean"==typeof i||"function"==typeof i?null:"string"==typeof i||"number"==typeof i||"bigint"==typeof i||i.constructor==String?g(null,i,null,null,i):y(i)?g(m,{children:i},null,null,null):void 0===i.constructor&&i.__b>0?g(i.type,i.props,i.key,i.ref?i.ref:null,i.__v):i)?(i.__=n,i.__b=n.__b+1,f=H(i,u,r=t+p,h),i.__i=f,o=null,-1!==f&&(h--,(o=u[f])&&(o.__u|=a)),null==o||null===o.__v?(-1==f&&p--,"function"!=typeof i.type&&(i.__u|=s)):f!==r&&(f===r+1?p++:f>r?h>e-r?p+=f-r:p--:p=f<r&&f==r-1?f-r:0,f!==t+p&&(i.__u|=s))):(o=u[t])&&null==o.key&&o.__e&&(o.__e==n.__d&&(n.__d=w(o)),O(o,o,!1),u[t]=null,h--);if(h)for(t=0;t<c;t++)null!=(o=u[t])&&0==(o.__u&a)&&(o.__e==n.__d&&(n.__d=w(o)),O(o,o))}function C(n,l,u){var t,i;if("function"==typeof n.type){for(t=n.__k,i=0;t&&i<t.length;i++)t[i]&&(t[i].__=n,l=C(t[i],l,u));return l}return n.__e!=l&&(u.insertBefore(n.__e,l||null),l=n.__e),l&&l.nextSibling}function H(n,l,u,t){var i=n.key,o=n.type,r=u-1,f=u+1,e=l[u];if(null===e||e&&i==e.key&&o===e.type)return u;if(t>(null!=e&&0==(e.__u&a)?1:0))for(;r>=0||f<l.length;){if(r>=0){if((e=l[r])&&0==(e.__u&a)&&i==e.key&&o===e.type)return r;r--}if(f<l.length){if((e=l[f])&&0==(e.__u&a)&&i==e.key&&o===e.type)return f;f++}}return-1}function I(n,l,u){"-"===l[0]?n.setProperty(l,null==u?"":u):n[l]=null==u?"":"number"!=typeof u||v.test(l)?u:u+"px"}function j(n,l,u,t,i){var o;n:if("style"===l)if("string"==typeof u)n.style.cssText=u;else{if("string"==typeof t&&(n.style.cssText=t=""),t)for(l in t)u&&l in u||I(n.style,l,"");if(u)for(l in u)t&&u[l]===t[l]||I(n.style,l,u[l])}else if("o"===l[0]&&"n"===l[1])o=l!==(l=l.replace(/(PointerCapture)$|Capture$/,"$1")),l=l.toLowerCase()in n?l.toLowerCase().slice(2):l.slice(2),n.l||(n.l={}),n.l[l+o]=u,u?t?u.u=t.u:(u.u=Date.now(),n.addEventListener(l,o?D:A,o)):n.removeEventListener(l,o?D:A,o);else{if(i)l=l.replace(/xlink(H|:h)/,"h").replace(/sName$/,"s");else if("width"!==l&&"height"!==l&&"href"!==l&&"list"!==l&&"form"!==l&&"tabIndex"!==l&&"download"!==l&&"rowSpan"!==l&&"colSpan"!==l&&"role"!==l&&l in n)try{n[l]=null==u?"":u;break n}catch(n){}"function"==typeof u||(null==u||!1===u&&"-"!==l[4]?n.removeAttribute(l):n.setAttribute(l,u))}}function A(n){var l=this.l[n.type+!1];if(n.t){if(n.t<=l.u)return}else n.t=Date.now();return l(u.event?u.event(n):n)}function D(n){return this.l[n.type+!0](u.event?u.event(n):n)}function L(n,l,t,i,o,r,f,e,c,s){var a,h,p,v,_,b,g,w,x,P,S,$,C,H,I,j=l.type;if(void 0!==l.constructor)return null;128&t.__u&&(c=!!(32&t.__u),r=[e=l.__e=t.__e]),(a=u.__b)&&a(l);n:if("function"==typeof j)try{if(w=l.props,x=(a=j.contextType)&&i[a.__c],P=a?x?x.props.value:a.__:i,t.__c?g=(h=l.__c=t.__c).__=h.__E:("prototype"in j&&j.prototype.render?l.__c=h=new j(w,P):(l.__c=h=new k(w,P),h.constructor=j,h.render=q),x&&x.sub(h),h.props=w,h.state||(h.state={}),h.context=P,h.__n=i,p=h.__d=!0,h.__h=[],h._sb=[]),null==h.__s&&(h.__s=h.state),null!=j.getDerivedStateFromProps&&(h.__s==h.state&&(h.__s=d({},h.__s)),d(h.__s,j.getDerivedStateFromProps(w,h.__s))),v=h.props,_=h.state,h.__v=l,p)null==j.getDerivedStateFromProps&&null!=h.componentWillMount&&h.componentWillMount(),null!=h.componentDidMount&&h.__h.push(h.componentDidMount);else{if(null==j.getDerivedStateFromProps&&w!==v&&null!=h.componentWillReceiveProps&&h.componentWillReceiveProps(w,P),!h.__e&&(null!=h.shouldComponentUpdate&&!1===h.shouldComponentUpdate(w,h.__s,P)||l.__v===t.__v)){for(l.__v!==t.__v&&(h.props=w,h.state=h.__s,h.__d=!1),l.__e=t.__e,l.__k=t.__k,l.__k.forEach(function(n){n&&(n.__=l)}),S=0;S<h._sb.length;S++)h.__h.push(h._sb[S]);h._sb=[],h.__h.length&&f.push(h);break n}null!=h.componentWillUpdate&&h.componentWillUpdate(w,h.__s,P),null!=h.componentDidUpdate&&h.__h.push(function(){h.componentDidUpdate(v,_,b)})}if(h.context=P,h.props=w,h.__P=n,h.__e=!1,$=u.__r,C=0,"prototype"in j&&j.prototype.render){for(h.state=h.__s,h.__d=!1,$&&$(l),a=h.render(h.props,h.state,h.context),H=0;H<h._sb.length;H++)h.__h.push(h._sb[H]);h._sb=[]}else do{h.__d=!1,$&&$(l),a=h.render(h.props,h.state,h.context),h.state=h.__s}while(h.__d&&++C<25);h.state=h.__s,null!=h.getChildContext&&(i=d(d({},i),h.getChildContext())),p||null==h.getSnapshotBeforeUpdate||(b=h.getSnapshotBeforeUpdate(v,_)),T(n,y(I=null!=a&&a.type===m&&null==a.key?a.props.children:a)?I:[I],l,t,i,o,r,f,e,c,s),h.base=l.__e,l.__u&=-161,h.__h.length&&f.push(h),g&&(h.__E=h.__=null)}catch(n){l.__v=null,c||null!=r?(l.__e=e,l.__u|=c?160:32,r[r.indexOf(e)]=null):(l.__e=t.__e,l.__k=t.__k),u.__e(n,l,t)}else null==r&&l.__v===t.__v?(l.__k=t.__k,l.__e=t.__e):l.__e=z(t.__e,l,t,i,o,r,f,c,s);(a=u.diffed)&&a(l)}function M(n,l,t){l.__d=void 0;for(var i=0;i<t.length;i++)N(t[i],t[++i],t[++i]);u.__c&&u.__c(l,n),n.some(function(l){try{n=l.__h,l.__h=[],n.some(function(n){n.call(l)})}catch(n){u.__e(n,l.__v)}})}function z(n,u,t,i,o,r,f,e,c){var s,a,p,v,d,b,g,m=t.props,k=u.props,x=u.type;if("svg"===x&&(o=!0),null!=r)for(s=0;s<r.length;s++)if((d=r[s])&&"setAttribute"in d==!!x&&(x?d.localName===x:3===d.nodeType)){n=d,r[s]=null;break}if(null==n){if(null===x)return document.createTextNode(k);n=o?document.createElementNS("http://www.w3.org/2000/svg",x):document.createElement(x,k.is&&k),r=null,e=!1}if(null===x)m===k||e&&n.data===k||(n.data=k);else{if(r=r&&l.call(n.childNodes),m=t.props||h,!e&&null!=r)for(m={},s=0;s<n.attributes.length;s++)m[(d=n.attributes[s]).name]=d.value;for(s in m)d=m[s],"children"==s||("dangerouslySetInnerHTML"==s?p=d:"key"===s||s in k||j(n,s,null,d,o));for(s in k)d=k[s],"children"==s?v=d:"dangerouslySetInnerHTML"==s?a=d:"value"==s?b=d:"checked"==s?g=d:"key"===s||e&&"function"!=typeof d||m[s]===d||j(n,s,d,m[s],o);if(a)e||p&&(a.__html===p.__html||a.__html===n.innerHTML)||(n.innerHTML=a.__html),u.__k=[];else if(p&&(n.innerHTML=""),T(n,y(v)?v:[v],u,t,i,o&&"foreignObject"!==x,r,f,r?r[0]:t.__k&&w(t,0),e,c),null!=r)for(s=r.length;s--;)null!=r[s]&&_(r[s]);e||(s="value",void 0!==b&&(b!==n[s]||"progress"===x&&!b||"option"===x&&b!==m[s])&&j(n,s,b,m[s],!1),s="checked",void 0!==g&&g!==n[s]&&j(n,s,g,m[s],!1))}return n}function N(n,l,t){try{"function"==typeof n?n(l):n.current=l}catch(n){u.__e(n,t)}}function O(n,l,t){var i,o;if(u.unmount&&u.unmount(n),(i=n.ref)&&(i.current&&i.current!==n.__e||N(i,null,l)),null!=(i=n.__c)){if(i.componentWillUnmount)try{i.componentWillUnmount()}catch(n){u.__e(n,l)}i.base=i.__P=null,n.__c=void 0}if(i=n.__k)for(o=0;o<i.length;o++)i[o]&&O(i[o],l,t||"function"!=typeof n.type);t||null==n.__e||_(n.__e),n.__=n.__e=n.__d=void 0}function q(n,l,u){return this.constructor(n,u)}function B(n,t,i){var o,r,f,e;u.__&&u.__(n,t),r=(o="function"==typeof i)?null:i&&i.__k||t.__k,f=[],e=[],L(t,n=(!o&&i||t).__k=b(m,null,[n]),r||h,h,void 0!==t.ownerSVGElement,!o&&i?[i]:r?null:t.firstChild?l.call(t.childNodes):null,f,!o&&i?i:r?r.__e:t.firstChild,o,e),M(f,n,e)}l=p.slice,u={__e:function(n,l,u,t){for(var i,o,r;l=l.__;)if((i=l.__c)&&!i.__)try{if((o=i.constructor)&&null!=o.getDerivedStateFromError&&(i.setState(o.getDerivedStateFromError(n)),r=i.__d),null!=i.componentDidCatch&&(i.componentDidCatch(n,t||{}),r=i.__d),r)return i.__E=i}catch(l){n=l}throw n}},t=0,i=function(n){return null!=n&&null==n.constructor},k.prototype.setState=function(n,l){var u;u=null!=this.__s&&this.__s!==this.state?this.__s:this.__s=d({},this.state),"function"==typeof n&&(n=n(d({},u),this.props)),n&&d(u,n),null!=n&&this.__v&&(l&&this._sb.push(l),P(this))},k.prototype.forceUpdate=function(n){this.__v&&(this.__e=!0,n&&this.__h.push(n),P(this))},k.prototype.render=m,o=[],f="function"==typeof Promise?Promise.prototype.then.bind(Promise.resolve()):setTimeout,e=function(n,l){return n.__v.__b-l.__v.__b},S.__r=0,c=0,n.Component=k,n.Fragment=m,n.cloneElement=function(n,u,t){var i,o,r,f,e=d({},n.props);for(r in n.type&&n.type.defaultProps&&(f=n.type.defaultProps),u)"key"==r?i=u[r]:"ref"==r?o=u[r]:e[r]=void 0===u[r]&&void 0!==f?f[r]:u[r];return arguments.length>2&&(e.children=arguments.length>3?l.call(arguments,2):t),g(n.type,e,i||n.key,o||n.ref,null)},n.createContext=function(n,l){var u={__c:l="__cC"+c++,__:n,Consumer:function(n,l){return n.children(l)},Provider:function(n){var u,t;return this.getChildContext||(u=[],(t={})[l]=this,this.getChildContext=function(){return t},this.shouldComponentUpdate=function(n){this.props.value!==n.value&&u.some(function(n){n.__e=!0,P(n)})},this.sub=function(n){u.push(n);var l=n.componentWillUnmount;n.componentWillUnmount=function(){u.splice(u.indexOf(n),1),l&&l.call(n)}}),n.children}};return u.Provider.__=u.Consumer.contextType=u},n.createElement=b,n.createRef=function(){return{current:null}},n.h=b,n.hydrate=function n(l,u){B(l,u,n)},n.isValidElement=i,n.options=u,n.render=B,n.toChildArray=function n(l,u){return u=u||[],null==l||"boolean"==typeof l||(y(l)?l.some(function(l){n(l,u)}):u.push(l)),u}});
//# sourceMappingURL=preact.umd.js.map
