from flask import Flask, request

import models

app = Flask(__name__)

@app.route('/items', methods=['GET'])
def items():
    # inventory = models.get_trash() if ('trash' in request.args) else models.get_items()
    # if len(inventory):
    #     return {'inventory': inventory}, 200
    # return '', 204
    return {'inventory': []}, 200

@app.route('/item/<name>', methods=['GET'])
def item_by_name(name):
    result = models.get_item_by_name(name)
    if len(result):
        return {'results': result}, 200
    return 'Not Found', 404

@app.route('/nextday', methods=['POST'])
def next_day():
    result = models.set_degrade_items()
    if len(result):
        return {'results': result}, 200
    return 'Bad Request', 400    

if __name__ == "__main__":
    app.run(debug=True, host='0.0.0.0', port='5000')