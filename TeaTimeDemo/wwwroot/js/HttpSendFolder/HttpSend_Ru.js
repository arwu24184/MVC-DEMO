$(document).ready(function () {


    //@Html.AntiForgeryToken()

    //如果要開啟驗證
    //控制器該fction要加上 [ValidateAntiForgeryToken]

    //alert("  FetchTester - htmlCs 放進 @Html.AntiForgeryToken()  ");


    }
);



class HttpSend_RuApi {

    //內部用------------------------------------------------------------------------------------------//
    static buildUrl(area, controller, action, body = {}) {
        const path = [area, controller, action].filter(part => part).join('/');

        let queryString = this.toQueryString(body);

        return queryString ? `/${path}?${queryString}` : `/${path}`;
    }

    static toQueryString(obj, prefix = '') {
        const pairs = [];

        for (const key in obj) {
            if (!Object.prototype.hasOwnProperty.call(obj, key)) continue;

            const value = obj[key];
            const isIdentifier = /^[a-zA-Z_$][0-9a-zA-Z_$]*$/.test(key);


            let fullKey;
            if (prefix === '') {
                fullKey = key;
            } else if (isIdentifier) {
                fullKey = `${prefix}.${key}`;
            } else {
                fullKey = `${prefix}[${key}]`;
            }

            if (Array.isArray(value)) {
                for (const v of value) {
                    if (typeof v === 'object' && v !== null) {
                        // 如果陣列內是物件，遞迴處理，加入索引避免名稱重複
                        const index = value.indexOf(v);
                        pairs.push(...this.toQueryString(v, `${fullKey}[${index}]`).split('&'));
                    } else {
                        pairs.push(`${encodeURIComponent(fullKey)}=${encodeURIComponent(v)}`);
                    }
                }
            } else if (typeof value === 'object' && value !== null) {
                // 遞迴展開巢狀物件
                pairs.push(...this.toQueryString(value, fullKey).split('&'));
            } else {
                pairs.push(`${encodeURIComponent(fullKey)}=${encodeURIComponent(value)}`);
            }
        }

        return pairs.join('&');
    }

    //內部用------------------------------------------------------------------------------------------//

    static handleResponse(response) {
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        return response.json();
    }

    static request(method, area, controller, action, body = {}, onSuccess = null, onError = null) {
        const url = (method === 'GET' || method === 'DELETE')
            ? this.buildUrl(area, controller, action, body)
            : this.buildUrl(area, controller, action);

        const fetchOptions = {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            ...((method !== 'GET' && method !== 'DELETE') && { body: JSON.stringify(body) })
        };

        fetch(url, fetchOptions)
            .then(this.handleResponse)
            .then(data => {
                if (typeof onSuccess === 'function') onSuccess(data);
                else console.log('Success:', data);
            })
            .catch(error => {
                if (typeof onError === 'function') onError(error);
                else console.error('Error:', error);
            });
    }

    //=========[調用類  Fetch 傳遞用法]=================================================================================//

    static get(area, controller, action, body = {}, onSuccess = null, onError = null) {
        this.request('GET', area, controller, action, body, onSuccess, onError);
    }

    static post(area, controller, action, body = {}, onSuccess = null, onError = null) {
        this.request('POST', area, controller, action, body, onSuccess, onError);
    }

    static put(area, controller, action, body = {}, onSuccess = null, onError = null) {
        this.request('PUT', area, controller, action, body, onSuccess, onError);
    }

    static patch(area, controller, action, body = {}, onSuccess = null, onError = null) {
        this.request('PATCH', area, controller, action, body, onSuccess, onError);
    }

    static delete(area, controller, action, body = {}, onSuccess = null, onError = null) {
        this.request('DELETE', area, controller, action, body, onSuccess, onError);
    }

    //=========[調用類  Submit 傳遞用法]=================================================================================//


    static Submit_Form(method, url) {
        const existing = document.getElementById('__Submit_Form__');
        if (existing) existing.remove();

        const form = document.createElement('form');
        form.method = method.toUpperCase();

        // 拆解 baseUrl 與查詢參數
        const [baseUrl, queryString] = url.split('?');
        form.action = baseUrl;
        form.id = '__Submit_Form__';

        // 把 query string 裡的參數塞進 form 的 hidden inputs
        const queryParams = new URLSearchParams(queryString || '');

        const appendFields = (formData, name, value) => {
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = name;
            input.value = value;
            formData.appendChild(input);
        };

        for (const [key, value] of queryParams.entries()) {
            appendFields(form, key, value);
        }

        document.body.appendChild(form);
        form.submit();
    }


    static Submit_Post(area, controller, action, data) {
        const url = this.buildUrl(area, controller, action, data);
        this.Submit_Form('POST', url);
    }

    static Submit_Get(area, controller, action, data) {
        const url = this.buildUrl(area, controller, action, data);
        this.Submit_Form('GET', url);
    }

}


